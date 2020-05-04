import { Component, OnInit, ViewChild } from '@angular/core';
import { User } from 'src/app/_models/user';
import { Router, ActivatedRoute } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent implements OnInit {

  user:User;

  @ViewChild('editForm',{static: false}) editForm: NgForm;

  constructor(private route: ActivatedRoute ,private alerify: AlertifyService) { }

  ngOnInit() {
    this.route.data.subscribe(data =>{
      this.user = data.user;
    });
  }

  updateUser(){
    console.log(this.user);
    this.alerify.success('Profil pomyślnie zaktualizowany');
    this.editForm.reset(this.user);
  }

}
