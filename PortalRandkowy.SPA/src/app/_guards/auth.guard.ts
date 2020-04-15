import { Injectable } from "@angular/core";
import { CanActivate, Router } from "@angular/router";
import { AuthService } from "../_services/auth.service";
import { AlertifyService } from "../_services/alertify.service";

@Injectable({
  providedIn: "root",
})
export class AuthGuard implements CanActivate {
  //to jest po to aby blokowalo reczne wpisywanie do podstron ktore nie powinny byc aktywne

  constructor(
    private authService: AuthService,
    private router: Router,
    private alertify: AlertifyService
  ) {}

  canActivate(): boolean {
    if (this.authService.loggedIn()) {
      // jesli jestes zalogowany to zwroci true i przezuci cie na strone
      return true;
    }

    this.alertify.error("Nie masz uprawnien"); // jesli nie to wyswietli sie blad i przekierowuje nas do strony glownej
    this.router.navigate(["/home"]);
    return false;
  }
}
