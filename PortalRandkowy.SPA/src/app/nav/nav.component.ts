import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router, ActivatedRoute } from '@angular/router';


@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
})
export class NavComponent implements OnInit {

  model: any = {};
  photoUrl: string;

  constructor(
    public authService: AuthService,
    private alertify: AlertifyService,
    private router: Router,
  ) {}

  ngOnInit() {
    this.authService.currentPhotoUrl.subscribe(photoUrl => this.photoUrl = photoUrl);
  }

  login() {
    this.authService.login(this.model).subscribe(
      (next) => {
        this.alertify.success('Zalogowałeś sie do aplikacji');
      },
      (error) => {
        this.alertify.error(error);
      },
      () => {
        // deklarujemy niewiadoma zmienna aby po akceptacji wywolac ja czyli nawigowac po kliknieciu do strony /uzytkownicy
        this.router.navigate(['/uzytkownicy']);
      }
    );
  }

  loggedIn() {
    return this.authService.loggedIn();
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.authService.currentUser = null;
    this.authService.decodedToken = null;
    this.alertify.message('Zostałeś wylogowany');
    this.router.navigate(['/home']); // po wylogowaniu nawigujemy strone do /home
  }

}
