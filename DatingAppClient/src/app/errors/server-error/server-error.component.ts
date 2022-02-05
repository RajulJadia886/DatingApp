import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-server-error',
  templateUrl: './server-error.component.html',
  styleUrls: ['./server-error.component.css']
})
export class ServerErrorComponent implements OnInit {
  error:any;

  constructor(private router: Router) { 
    //get data passed if any while routing
    const navigation = this.router.getCurrentNavigation();
    //optional chaining while getting extras data since it can be null and result an error.
    this.error = navigation?.extras?.state?.error;
  }

  ngOnInit(): void {
  }

}
