import { Component } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { CustomValidators } from 'ng2-validation';
import { RegisterDto } from "../identity.model";

@Component({
  selector: 's-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})

export class RegisterComponent {

  registerDto: RegisterDto = new RegisterDto();

  constructor(private http: HttpClient) { }

}
