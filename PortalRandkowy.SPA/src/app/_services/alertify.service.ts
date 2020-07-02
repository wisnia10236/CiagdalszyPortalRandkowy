import { Injectable } from '@angular/core';

declare let alertify: any;

@Injectable({
  providedIn: 'root'
})
export class AlertifyService {

constructor() { }

success(message: string) {
  alertify.success(message);
}

error(message: string) {
  alertify.error(message);
}

warning(message: string) {
  alertify.warning(message);
}

message(message: string) {
  alertify.message(message);
}

// tslint:disable-next-line: max-line-length
confirm(message: string , okCallBack: () => any) {      // wyswietla nam jakis message i jesli jest odzew (callback) to wyswietla nam go i zatwierdza
  alertify.confirm(message, (e) => {
    if (e) {
      okCallBack();
    } else {}
  });
}

}
