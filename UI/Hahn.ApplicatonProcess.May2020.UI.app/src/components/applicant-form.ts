import { I18N } from 'aurelia-i18n';
import { Router } from 'aurelia-router';
import { BootstrapFormRenderer } from './form-renderer';
import { ApplicantForCreationDto } from './../models/applicantForCreationDto';
import { inject } from 'aurelia-dependency-injection';
import { BindingEngine, observable } from 'aurelia-framework';
import { DialogService } from 'aurelia-dialog';
import { Popup } from './popup';

import {
  ValidationControllerFactory,
  ValidationRules,
  validateTrigger,
  validationMessages,
} from 'aurelia-validation';
import { ApplicantService } from 'services/applicantService';

@inject(ValidationControllerFactory, ApplicantService, Router, I18N, BindingEngine, DialogService)
export class ApplicantForm {
  disableSubmit = true;
  enableReset = false;
  controller = null;
  validator = null;
  router: Router;
  applicantService;
  i18N: I18N;
  bindingEngine: BindingEngine;
  subscription;
  applicant: ApplicantForCreationDto = <ApplicantForCreationDto>{};
  nameIsDirty: boolean;
  familyNameIsDirty: boolean;
  addressIsDirty: boolean;
  emailAddressIsDirty: boolean;
  isDirty: boolean;
  dialogService: any;
  constructor(controllerFactory, applicantService, router, i18N, bindingEngine, dialogService) {
    this.controller = controllerFactory.createForCurrentScope();
    this.controller.addRenderer(new BootstrapFormRenderer())
    this.controller.validateTrigger = validateTrigger.change;
    this.applicantService = applicantService;
    this.router = router;
    this.i18N = i18N;
    this.bindingEngine = bindingEngine;
    this.dialogService = dialogService;
  }
  updateDirty() {
    this.isDirty = this.nameIsDirty || this.familyNameIsDirty || this.emailAddressIsDirty || this.addressIsDirty;
  }
  displayPrompt() {
    this.dialogService.open({ viewModel: Popup, model: this.i18N.tr('home.resetmessage'), lock: false }).whenClosed(response => {
      if (!response.wasCancelled) {
        this.applicant = <ApplicantForCreationDto>{};
        this.isDirty = false;
      } else {

      }
      console.log(response.output);
    });
  }
  attached() {
    this.subscription =
      this.bindingEngine
        .propertyObserver(this.applicant, 'name')
        .subscribe((newValue, oldValue) => {
          this.nameIsDirty = newValue?.length > 0;
          console.log(this.nameIsDirty);
          this.updateDirty();
        });
    this.bindingEngine
      .propertyObserver(this.applicant, 'familyName')
      .subscribe((newValue, oldValue) => {
        this.familyNameIsDirty = newValue?.length > 0;
        this.updateDirty();
      });
    this.bindingEngine
      .propertyObserver(this.applicant, 'emailAddress')
      .subscribe((newValue, oldValue) => {
        this.emailAddressIsDirty = newValue?.length > 0;
        this.updateDirty();
      });
    this.bindingEngine
      .propertyObserver(this.applicant, 'address')
      .subscribe((newValue, oldValue) => {
        this.addressIsDirty = newValue?.length > 0;
        this.updateDirty();
      });
      this.bindingEngine
      .propertyObserver(this.applicant, 'age')
      .subscribe((newValue, oldValue) => {
        this.addressIsDirty = newValue > 0;
        this.updateDirty();
      });
  }

  detached() {
    this.subscription.dispose();
  }

  public bind() {
    if (this.applicant) {
      validationMessages['required'] = this.i18N.tr('home.requiredfield');
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

  createApplicant() {
    this.applicant.age = Number.parseInt(this.applicant.age.toString());
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
    this.displayPrompt();
  }

}
