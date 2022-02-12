using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingAppAPI.Data;
using DatingAppAPI.DTOs;
using DatingAppAPI.Entities;
using DatingAppAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingAppAPI.Controllers
{
   
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
       
        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }
    
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers(){
            
            return  Ok(await _userRepository.GetMembersAsync());
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username){

            return Ok(await _userRepository.GetMemberAsync(username));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto){

            //get username from token claims.
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            //get user by username.
            var user = await _userRepository.GetUserByUsernameAsync(username);

            //map memberupdatedto to appuser.
            _mapper.Map(memberUpdateDto, user);

            //update user.
            _userRepository.Update(user);

            //if saveallasync is successful then return no content since in frontend we are already showing the updated member.
            if(await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update user.");
            
        }
    }
}