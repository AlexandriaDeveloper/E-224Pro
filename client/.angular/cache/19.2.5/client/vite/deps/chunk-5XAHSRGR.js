import {
  ElementRef
} from "./chunk-V6F5SRNB.js";

// node_modules/@angular/cdk/fesm2022/element-08c51fcd.mjs
function coerceNumberProperty(value, fallbackValue = 0) {
  if (_isNumberValue(value)) {
    return Number(value);
  }
  return arguments.length === 2 ? fallbackValue : 0;
}
function _isNumberValue(value) {
  return !isNaN(parseFloat(value)) && !isNaN(Number(value));
}
function coerceElement(elementOrRef) {
  return elementOrRef instanceof ElementRef ? elementOrRef.nativeElement : elementOrRef;
}

export {
  coerceNumberProperty,
  _isNumberValue,
  coerceElement
};
//# sourceMappingURL=chunk-5XAHSRGR.js.map
