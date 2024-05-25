import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export class ConfirmPasswordValidators {
    static match(controleName: string , MatchingControleName: string): ValidatorFn
    {
        return (group: AbstractControl): ValidationErrors | null =>
        {
            const Password = group.get(controleName)
            const ConfirmPassword = group.get(MatchingControleName)
            if (!Password || !ConfirmPassword)
            {
                return {ControlNotFound: false}
            }
            const error = (Password.value === ConfirmPassword.value)?
            null :
            {
                Notmatch: true
            }
            ConfirmPassword.setErrors(error)
            return error
        }
    }
}

