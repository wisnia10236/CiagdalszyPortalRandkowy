import { Injectable } from "@angular/core";
import {
  HttpInterceptor,
  HttpRequest,
  HttpEvent,
  HttpHandler,
  HttpErrorResponse,
  HTTP_INTERCEPTORS,
} from "@angular/common/http";
import { Observable, throwError } from "rxjs";
import { catchError } from "rxjs/operators";

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error) => {
        if (error instanceof HttpErrorResponse) {
          const applicationError = error.headers.get("Application-Error");

          if (applicationError) {
            console.error(applicationError);
            return throwError(applicationError);
          }

          const serverError = error.error; // dla errora(httpErrorResponse) przekazujemy parametr error dla tablic errors
          let errors = "";

          if (serverError && typeof serverError === "object") {
            // sprawdzamy czy jest servererror i czy serverError to object(czyli cos dostajemy w przegladarce)
            for (const key in serverError) {
              if (serverError[key]) {
                //jesli jest taki servererror o tym kluczu to dodajemy go do errors'ów
                errors += serverError[key] + "\n";
              }
            }
          }
          return throwError(errors || serverError || "Server Error");
        }
      })
    );
  }
}

export const ErrorInterceptorProvider = {
  //pobieramy bledy z Api wyswietlone w przesylaniu abysmy mogli je odczytac w angularze
  provide: HTTP_INTERCEPTORS,
  useClass: ErrorInterceptor,
  multi: true,
};
