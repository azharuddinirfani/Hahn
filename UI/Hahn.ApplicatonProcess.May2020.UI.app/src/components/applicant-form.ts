import { Router } from 'aurelia-router';
import { BootstrapFormRenderer } from './form-renderer';
import { ApplicantForCreationDto } from './../models/applicantForCreationDto';
import { inject } from 'aurelia-dependency-injection';
import { observable } from 'aurelia-framework';

import {
  ValidationControllerFactory,
  ValidationRules,
  validateTrigger,
} from 'aurelia-validation';
import { ApplicantService } from 'services/applicantService';

@inject(ValidationControllerFactory, ApplicantService,Router)
export class ApplicantForm {
  disableSubmit = true;
  enableReset = false;
  controller = null;
  validator = null;
  router:Router;
  applicantService;
  @observable applicant: ApplicantForCreationDto = <ApplicantForCreationDto>{};
  constructor(controllerFactory, applicantService,router) {
    this.controller = controllerFactory.createForCurrentScope();
    this.controller.addRenderer(new BootstrapFormRenderer())
    this.controller.validateTrigger = validateTrigger.change;
    this.applicantService = applicantService;
    this.router = router;
  }


  public bind() {
    if (this.applicant) {
      ValidationRules
        .ensure('name')
        .required()
        .minLength(5)
        .ensure('familyName')
        .required()
        .minLength(5)
        .ensure('address')
        .required()
        .minLength(10)
        .ensure('countryOfOrigin')
        .required()
        .ensure('emailAddress')
        .required()
        .email()
        .ensure('age')
        .required()
        .min(20)
        .max(60)
        .on(this.applicant);

      if (this.controller)
        this.controller.validate();
    }

  }

  applicantChanged(newValue, oldValue) {
    // this will fire whenever the 'color' property changes
    console.log(this.applicant);

    if (this.controller) {
      this.controller.validate();
    }
    if (this.applicant) {
      this.enableReset = this.applicant.name?.length > 0
        || this.applicant.familyName?.length > 0
        || this.applicant.address?.length > 0
        || this.applicant.countryOfOrigin?.length > 0
        || this.applicant.emailAddress?.length > 0
        || this.applicant.age > 0;

      console.log(this.applicant);
    }
  }

  createApplicant() {
    this.applicant.age =Number.parseInt(this.applicant.age.toString());
    this.applicantService
      .addApplicant(this.applicant)
      .then(x => {
        this.router.navigate("success");
      })
      .catch(x => {
        console.log('error');
        console.log(x);
      })
      ;
  }
  reset() {
    this.controller.reset();
    this.applicant = <ApplicantForCreationDto>{};
  }

}
// ValidationRules
//       .ensure( a => a.applicant.).required()
//       .ensure("familyName").required()
//       .on(ApplicantForm);
