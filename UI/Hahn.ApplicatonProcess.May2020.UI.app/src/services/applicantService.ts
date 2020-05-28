import { autoinject } from 'aurelia-framework';
import { HttpClient, json } from 'aurelia-fetch-client';

@autoinject
export class ApplicantService {
  constructor(private http: HttpClient) {
    this.http = http;
    const baseUrl = 'http://localhost:51044/api/applicants/';
    http.configure(config => {
      config
        .withBaseUrl(baseUrl)
        .withDefaults({
          //credentials: 'same-origin',
          headers: {
            'Accept': 'application/json',
            'X-Requested-With': 'Fetch'
          }
        })
        ;

    });
  }
  getApplicant(id) {
    return this.http.fetch(id)
      .then(response => response.json())
      .then(applicant => {
        console.log(applicant);
        // return card;
      });
  }

  addApplicant(applicant) {
    console.log(applicant);
    return this.http.fetch('',
      {
        method: 'post',
        body: json(applicant)
      })
      .then(response => { 
        if(!response.ok)
         throw new Error('failed');
         return response.json();
      })
      ;
  }
}


