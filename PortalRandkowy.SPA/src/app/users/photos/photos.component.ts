import { Component, OnInit, Input } from '@angular/core';
import { Photo } from '../../_models/photo';
import { FileUploader } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';
import { AuthService } from 'src/app/_services/auth.service';

const URL = 'https://evening-anchorage-3159.herokuapp.com/api/';
@Component({
  selector: 'app-photos',
  templateUrl: './photos.component.html',
  styleUrls: ['./photos.component.css']
})
export class PhotosComponent implements OnInit {

  @Input() photos: Photo[];     // musi dostac od useredit zdjecia

  uploader: FileUploader;
  hasBaseDropZoneOver: false;
  response: string;
  baseUrl = environment.apiUrl;

  constructor(private authService: AuthService) { }



  ngOnInit() {
    this.initializeUploader();
  }

  public fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/' + this.authService.decodedToken.nameid + '/photos',
      authToken: 'Bearer ' + localStorage.getItem('token'),
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024,
    });


    this.uploader.onAfterAddingFile = (file) => { file.withCredentials = false; };   // po dodaniu zdjec wyswietla sie w galerii te zdj co dodalismy

    this.uploader.onSuccessItem = (item, response, status, headres) => {            //jesli jest sukces po wrzuceniu zdj to ma byc item, response ,status i headers
      if(response){           //jesli jest odpowiedz to
        const res: Photo = JSON.parse(response);        //parsujemy do stalej odpowiedz ze zdjecia
        const photo = {           //przekazujemy z odp do stalej jej dane
          id: res.id,
          url: res.url,
          dateAdded: res.dateAdded,
          description: res.description,
          isMain: res.isMain
        };
        this.photos.push(photo);        //wysylamy nasza stala do kolekcji zdj (photos) aby on mogl przekazac go do api
      }
    };

  }



}