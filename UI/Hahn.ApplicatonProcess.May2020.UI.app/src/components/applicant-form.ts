import { I18N } from 'aurelia-i18n';
import { Router } from 'aurelia-router';
import { BootstrapFormRenderer } from './form-renderer';
import { ApplicantForCreationDto } from './../models/applicantForCreationDto';
import { inject } from 'aurelia-dependency-injection';
import { observable } from 'aurelia-framework';

import {
  ValidationControllerFactory,
  ValidationRules,
  validateTrigger,
  validationMessages,
} from 'aurelia-validation';
import { ApplicantService } from 'services/applicantService';

@inject(ValidationControllerFactory, ApplicantService,Router,I18N, validationMessages)
export class ApplicantForm {
  disableSubmit = true;
  enableReset = false;
  controller = null;
  validator = null;
  router:Router;
  applicantService;
  i18N: I18N;
  @observable applicant: ApplicantForCreationDto = <ApplicantForCreationDto>{};
  constructor(controllerFactory, applicantService,router,i18N) {
    this.controller = controllerFactory.createForCurrentScope();
    this.controller.addRenderer(new BootstrapFormRenderer())
    this.controller.validateTrigger = validateTrigger.change;
    this.applicantService = applicantService;
    this.router = router;
    this.i18N = i18N;
  }


  public bind() {
    if (this.applicant) {
      validationMessages['required'] =this.i18N.tr('home.requiredfield');
      ValidationRules
      
        .ensure('name')
        .displayName(this.i18N.tr('home.name'))
        .required()
        .minLength(5)
        .ensure('familyName')
        .displayName(this.i18N.tr('home.familyname'))
        .required()
        .minLength(5)
        .ensure('address')
        .displayName(this.i18N.tr('home.address'))
        .required()
        .minLength(10)
        .ensure('countryOfOrigin')
        .displayName(this.i18N.tr('home.countryoforigin'))
        .required()
        .ensure('emailAddress')
        .displayName(this.i18N.tr('home.emailaddress'))
        .required()
        .email()
        .ensure('age')
        .displayName(this.i18N.tr('home.age'))
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
