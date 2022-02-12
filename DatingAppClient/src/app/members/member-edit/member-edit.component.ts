import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Member } from 'src/app/models/member';
import { User } from 'src/app/models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
 
  //view child decorator to access our form through template reference variable.
  @ViewChild('editForm') editForm: NgForm;

  //host listener for providing angular to access browser events.
  @HostListener('window: beforeunload',['$event']) unloadNotification($event: any){
    if(this.editForm.dirty){
      $event.returnValue = true;
    }
  }

  user:User;
  member: Member;

  constructor(private accountService: AccountService, private memberService: MembersService,
     private toastr: ToastrService) {
       
      this.accountService.currentUser$.subscribe(user => {
         this.user = user;
        });
   }

  ngOnInit(): void {
    this.loadMember();
  }
  
  loadMember(){
    this.memberService.GetMember(this.user.username).subscribe(member => {
      this.member = member;
    });
  }

  updateMember(){
    this.memberService.UpdateMember(this.member).subscribe(()=>{
      this.toastr.success('Profile updated successfully.')
      //reseting the value to the current updated member after updating.
      this.editForm.reset(this.member);
    });
  }
}
