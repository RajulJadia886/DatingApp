using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingAppAPI.Data;
using DatingAppAPI.DTOs;
using DatingAppAPI.Entities;
using DatingAppAPI.Extensions;
using DatingAppAPI.Helpers;
using DatingAppAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingAppAPI.Controllers
{
   
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
       
        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            _photoService = photoService;
            _mapper = mapper;
            _userRepository = userRepository;
        }
    
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery]UserParams userParams){

            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            userParams.CurrentUsername = user.UserName;
            if(string.IsNullOrEmpty(userParams.Gender))
                userParams.Gender = user.Gender == "male" ? "female":"male";
            var users = await _userRepository.GetMembersAsync(userParams);
            Response.AddPaginationHeader(users.CurrentPage, users.PageSize,users.TotalCount,users.TotalPages);
            return  Ok(users);
        }

        [HttpGet("{username}", Name = "GetUser")]
        public async Task<ActionResult<MemberDto>> GetUser(string username){

            return Ok(await _userRepository.GetMemberAsync(username));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto){

            //get user by username.
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            //map memberupdatedto to appuser.
            _mapper.Map(memberUpdateDto, user);

            //update user.
            _userRepository.Update(user);

            //if saveallasync is successful then return no content since in frontend we are already showing the updated member.
            if(await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update user.");
            
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file){

            //authenticating user
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            //uploading photo in cloudinary
            var result = await  _photoService.AddPhotoAsync(file);

            //if error occurs return bad request.
            if(result.Error != null) return BadRequest(result.Error.Message);

            //create photo object update Url and public id.
            var photo = new Photo{
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            //if user has no existing photos upload the new file as main photo.
            if(user.Photos.Count == 0){
                photo.IsMain = true;
            }

            //add photo in user photos object.
            user.Photos.Add(photo);

            //save all changes and return photodto.
            if(await _userRepository.SaveAllAsync()){
                //created at route gives the status code as 201 and the location where the resource has been created.
                //Here we are creating the resouce at apiUrl/username and returning photodto object eg https://localhost:5001/api/Users/lisa.
                return CreatedAtRoute("GetUser", new {username = user.UserName},_mapper.Map<PhotoDto>(photo));
            }

            //if any problem occurs return bad request.
            return BadRequest("Problem in uploading photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<IActionResult> SetMainPhoto(int photoId){

            //find user.
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            
            //find photo.
            var photo = user.Photos.FirstOrDefault(photo => photo.Id == photoId);

            if(photo == null) return NotFound(); 

            //if the same photo is already main then return bad request.
            if(photo.IsMain) return BadRequest("This is already a main photo.");

            //find first photo of user that is the main photo.
            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);

            //if user has any main photo previously then make it false.
            if(currentMain != null) currentMain.IsMain = false;

            //after making previous ismain to false make the new photo as main.
            photo.IsMain = true;

            if(await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to set main photo.");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId){
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);
            if(photo == null) return NotFound();

            if(photo.IsMain) return BadRequest("You cannot delete your main photo.");
            if(photo.PublicId != null){
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if(result.Error != null) return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);
            if(await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to delete the photo.");
        } 
    }
}