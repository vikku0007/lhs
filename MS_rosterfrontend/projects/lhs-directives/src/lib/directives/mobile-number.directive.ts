//directive

import { Directive, HostListener } from '@angular/core';
import { NgControl } from '@angular/forms';

@Directive({
selector: '[formControlName][appMobileNumber]',
})
export class PhoneNumberDirective {

constructor(public ngControl: NgControl) { }

@HostListener('ngModelChange', ['$event'])
onModelChange(event) {
this.onInputChange(event, false);
}

@HostListener('keydown.backspace', ['$event'])
keydownBackspace(event) {
this.onInputChange(event.target.value, true);
}

onInputChange(event, backspace) {
let newVal='';

newVal = event;

newVal = newVal.replace(/^(\+[0-9]\d{0,1})/,'');
if(event!=null){
    newVal = newVal.replace(/\D/g, '');
    }
if (backspace && newVal.length <= 6) {
newVal = newVal.substring(0, newVal.length - 1);
}
if (newVal.length === 0) {
newVal = '';
} else if (newVal.length <= 3) {
newVal = newVal.replace(/^(\d{0,3})/, '+$1');
} else if (newVal.length <= 4) {
newVal = newVal.replace(/^(\d{0,3})(\d{0,3})/, '$1$2');
} else if (newVal.length <= 10) {
newVal = newVal.replace(/^(\d{0,3})(\d{0,3})(\d{0,3})/, '$1$2$3');
}
//else {
// newVal = newVal.substring(0, 10);
// newVal = newVal.replace(/^(\d{0,3})(\d{0,3})(\d{0,4})/, '($1) $2-$3');
// }
else {
newVal = newVal.replace(/^(\d{0,3})(\d{0,3})(.*)/, '$1 $2 $3');
}
this.ngControl.valueAccessor.writeValue(newVal);
}
}