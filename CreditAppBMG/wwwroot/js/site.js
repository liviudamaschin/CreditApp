// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
(function ($) {
    $.fn.setCursorToTextEnd = function () {
        var $initialVal = this.val();
        this.val($initialVal);
    };
})(jQuery);


function showErrors(errorMessage, errormap, errorlist) {
    var val = this;
    errormap.forEach(function (error, index) {
        val.settings.highlight.call(val, error.element, val.settings.errorClass, val.settings.validClass);
        $(error.element).siblings("span.field-validation-valid, span.field-validation-error").html($("<span></span>").html(error.message)).addClass("field-validation-error").removeClass("field-validation-valid").show();
    });
}

function fixValidFieldStyles($form, validator) {
    var errors = {};
    $form.find("input,select").each(function (index) {
        var name = $(this).attr("name");
        errors[name] = validator.errorsFor(name);
    });
    validator.showErrors(errors);
    var invalidFields = $form.find("." + validator.settings.errorClass);
    if (invalidFields.length) {
        invalidFields.each(function (index, field) {
            if ($(field).valid()) {
                $(field).removeClass(validator.settings.errorClass);
            }
        });
    }
}