import {
  RenderInstruction,
  ValidateResult
} from 'aurelia-validation';

export class BootstrapFormRenderer {
  render(instruction: RenderInstruction) {
    for (let { result, elements } of instruction.unrender) {
      for (let element of elements) {
        this.remove(element, result);
      }
    }

    for (let { result, elements } of instruction.render) {
      for (let element of elements) {
        this.add(element, result);
      }
    }
  }

  add(element: Element, result: ValidateResult) {
    if (result.valid) {
      return;
    }

    const formGroup = element.closest('.form-group');
    const formControl = element.closest('.form-control');

    if (!formGroup) {
      return;
    }

    // add the has-error class to the enclosing form-group div
    formControl.classList.add('is-invalid');

    // add help-block
    const message = document.createElement('span');
    message.className = 'is-invalid validation-message';
    message.textContent = result.message;
    message.id = `validation-message-${result.id}`;
    formGroup.appendChild(message);
  }

  remove(element: Element, result: ValidateResult) {
    if (result.valid) {
      return;
    }

    const formGroup = element.closest('.form-group');
    const formControl = element.closest('.form-control');
    if (!formGroup) {
      return;
    }

    // remove help-block
    const message = formGroup.querySelector(`#validation-message-${result.id}`);
    if (message) {
      formGroup.removeChild(message);

      // remove the has-error class from the enclosing form-group div
      if (formControl.querySelectorAll('.is-invalid.validation-message').length === 0) {
        formControl.classList.remove('is-invalid');
      }
    }
  }
}


