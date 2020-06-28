import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { User } from 'src/app/_models/user';
import { Router, ActivatedRoute } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { NgForm } from '@angular/forms';
import { UserService } from 'src/app/_services/user.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent implements OnInit {

  user: User;

  @ViewChild('editForm', {static: false}) editForm: NgForm;
  @HostListener('window:beforeunload', ['$event'])
  uploadNotification($event: any)
  {
    if (this.editForm.dirty)
    {
      $event.returnValue = true;
    }
  }

  constructor(private route: ActivatedRoute ,
              private alerify: AlertifyService,
              private userService: UserService ,
              private authService: AuthService ) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user = data.user;
    });
  }

  updateUser(){

    // tslint:disable-next-line:max-line-length
    this.userService.updateUser(this.authService.decodedToken.nameid, this.user)         // odwolujemy sie do serwisu z parametramy z id oraz user aby mogl zupdatowac zmiany na bazie dancyh
          .subscribe(next => {            // jest to zmiana na bazie danych i jezeli wszystko bedzie ok to wyswietla alerify
                 this.alerify.success('Profil pomyślnie zaktualizowany');
                 this.editForm.reset(this.user);
                  }, error => {
                    this.alerify.error(error);
                  });


  }

  updateMainPhoto(photoUrl)
  {
    this.user.photoUrl = photoUrl;
  }

}
