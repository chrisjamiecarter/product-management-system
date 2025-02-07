window.FormLoading = {
  initialize: function (formId, options = {}) {
    const defaults = {
      submitButtonId: null, // If null, will look for first submit button
      validateForm: true,
      disableOnSubmit: true
    };

    const settings = { ...defaults, ...options };
    const form = document.getElementById(formId);

    if (!form) {
      console.warn(`Form with ID '${formId}' not found`);
      return;
    }

    const submitButton = settings.submitButtonId
      ? document.getElementById(settings.submitButtonId)
      : form.querySelector('button[type="submit"]');

    if (!submitButton) {
      console.warn('Submit button not found');
      return;
    }

    // Create and insert loading spinner if it doesn't exist
    if (!submitButton.querySelector('.button-content')) {
      const originalContent = submitButton.innerHTML;
      submitButton.innerHTML = `
        <span class="button-content">${originalContent}</span>
        <span class="button-loading d-none">
          <div class="d-flex justify-content-center align-items-center">
            <div class="spinner-border" role="status">
              <span class="visually-hidden">Loading...</span>
            </div>
          </div>
        </span>
        `;
    }

    const buttonContent = submitButton.querySelector('.button-content');
    const buttonLoading = submitButton.querySelector('.button-loading');

    form.addEventListener('submit', function (e) {
      if (settings.validateForm && !form.checkValidity()) {
        return;
      }

      if (settings.disableOnSubmit) {
        submitButton.disabled = true;
      }

      buttonContent.classList.add('d-none');
      buttonLoading.classList.remove('d-none');
    });
  }
};