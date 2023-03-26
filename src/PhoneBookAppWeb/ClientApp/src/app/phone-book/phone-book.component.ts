import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { first } from 'rxjs';

@Component({
  selector: 'phone-book',
  templateUrl: './phone-book.component.html',
  styleUrls: ['./phone-book.component.css']
})
export class PhoneBookComponent {
  public contacts: Contact[] = [];
  searchLastname: string = "";
  myAppUrl: string = "";
  myHttp: HttpClient | undefined; 

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.myAppUrl = baseUrl;
    this.myHttp = http;
    this.getAllContacts();
  }

  getAllContacts() {
    this.myHttp?.get<Contact[]>(this.myAppUrl + 'contacts').subscribe(result => {
      this.contacts = result;
    }, error => console.error(error));
  }

  getContactByName() {
    this.myHttp?.get<Contact[]>(this.myAppUrl + 'contacts/' + this.searchLastname).subscribe(result => {
      this.contacts = result;
    }, error => console.error(error));
  }

  delete(contactId: number) {
    if (confirm("Do you want to delete contact " + contactId)) {
      let self = this;
      let headers = new HttpHeaders();
      headers.append('Content-Type', 'application/json; charset=utf-8');
      this.myHttp?.delete(this.myAppUrl + "contacts/" + contactId, { headers: headers })
        .subscribe(result => {
            self.getAllContacts();
        });
    }
  }
}

interface Contact {
  contactId: number;
  firstName: string;
  lastName: string;
  phoneNumber: number;
}
