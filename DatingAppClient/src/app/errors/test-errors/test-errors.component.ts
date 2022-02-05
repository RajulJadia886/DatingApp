import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-test-errors',
  templateUrl: './test-errors.component.html',
  styleUrls: ['./test-errors.component.css']
})
export class TestErrorsComponent implements OnInit {

  baseUrl = "https://localhost:5001/api/";
  validationErrors : string[] = [];
  constructor(private http : HttpClient) { }

  ngOnInit(): void {
  }

  //Get 500 Internal Server Error. ForEg: When there is any exception at server side.
  get500Error(){
    this.http.get(this.baseUrl+'buggy/server-error').subscribe(response => {
      console.log(response);
    },error=>{
      console.log(error);
    });
  }

  //Get 400 Bad Request Error.
  get400Error(){
    this.http.get(this.baseUrl+'buggy/bad-request').subscribe(response => {
      console.log(response);
    },error=>{
      console.log(error);
    });
  }

  //Get 401 Unauthorized Error.
  get401Error(){
    this.http.get(this.baseUrl+'buggy/auth').subscribe(response => {
      console.log(response);
    },error=>{
      console.log(error);
    });
  }

  //Get 404 Not-Found Error.
  get404Error(){
    this.http.get(this.baseUrl+'buggy/not-found').subscribe(response => {
      console.log(response);
    },error=>{
      console.log(error);
    });
  }

  //Get Validation Error Eg: when we submit empty user for registration.
  get400VaidationError(){
    this.http.post(this.baseUrl+'account/register',{}).subscribe(response => {
      console.log(response);
    },error=>{
      console.log(error);
      this.validationErrors = error;
    });
  }
}
