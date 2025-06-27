import {
  MAT_SELECT_CONFIG,
  MAT_SELECT_SCROLL_STRATEGY,
  MAT_SELECT_SCROLL_STRATEGY_PROVIDER,
  MAT_SELECT_SCROLL_STRATEGY_PROVIDER_FACTORY,
  MAT_SELECT_TRIGGER,
  MatSelect,
  MatSelectChange,
  MatSelectModule,
  MatSelectTrigger
} from "./chunk-UA7UEZQ7.js";
import {
  MatOptgroup,
  MatOption
} from "./chunk-W43VMNNU.js";
import "./chunk-5GAE3HLD.js";
import "./chunk-T5BMGDIF.js";
import "./chunk-J2WI3QDB.js";
import "./chunk-6GM3VVCS.js";
import "./chunk-4XZGEDVP.js";
import "./chunk-SZS4RJEH.js";
import "./chunk-UFGSV2QV.js";
import "./chunk-A2WB37WA.js";
import "./chunk-OIZAD6NR.js";
import {
  MatError,
  MatFormField,
  MatHint,
  MatLabel,
  MatPrefix,
  MatSuffix
} from "./chunk-QTFXBRRI.js";
import "./chunk-UDU42JBG.js";
import "./chunk-RQEAODYP.js";
import "./chunk-JCMB2HVW.js";
import "./chunk-BMNIKX2V.js";
import "./chunk-2V2JD4M6.js";
import "./chunk-43XUVGIN.js";
import "./chunk-6CPOO2TF.js";
import "./chunk-UU5Z7QKS.js";
import "./chunk-X3P5GA7C.js";
import "./chunk-YY2C6AUH.js";
import "./chunk-M3HR6BUY.js";
import "./chunk-ANP26CVA.js";
import "./chunk-GN3GCMDU.js";
import "./chunk-65RJ5ZZ2.js";
import "./chunk-H2MPCWOQ.js";
import "./chunk-DTWIFKD7.js";
import "./chunk-YLL6V4HN.js";
import "./chunk-YNRXC4MO.js";
import "./chunk-GCFHXMNB.js";
import "./chunk-UDA4GOX7.js";
import "./chunk-RQWDYR4S.js";

// node_modules/@angular/material/fesm2022/select.mjs
var matSelectAnimations = {
  // Represents
  // trigger('transformPanelWrap', [
  //   transition('* => void', query('@transformPanel', [animateChild()], {optional: true})),
  // ])
  /**
   * This animation ensures the select's overlay panel animation (transformPanel) is called when
   * closing the select.
   * This is needed due to https://github.com/angular/angular/issues/23302
   */
  transformPanelWrap: {
    type: 7,
    name: "transformPanelWrap",
    definitions: [{
      type: 1,
      expr: "* => void",
      animation: {
        type: 11,
        selector: "@transformPanel",
        animation: [{
          type: 9,
          options: null
        }],
        options: {
          optional: true
        }
      },
      options: null
    }],
    options: {}
  },
  // Represents
  // trigger('transformPanel', [
  //   state(
  //     'void',
  //     style({
  //       opacity: 0,
  //       transform: 'scale(1, 0.8)',
  //     }),
  //   ),
  //   transition(
  //     'void => showing',
  //     animate(
  //       '120ms cubic-bezier(0, 0, 0.2, 1)',
  //       style({
  //         opacity: 1,
  //         transform: 'scale(1, 1)',
  //       }),
  //     ),
  //   ),
  //   transition('* => void', animate('100ms linear', style({opacity: 0}))),
  // ])
  /** This animation transforms the select's overlay panel on and off the page. */
  transformPanel: {
    type: 7,
    name: "transformPanel",
    definitions: [{
      type: 0,
      name: "void",
      styles: {
        type: 6,
        styles: {
          opacity: 0,
          transform: "scale(1, 0.8)"
        },
        offset: null
      }
    }, {
      type: 1,
      expr: "void => showing",
      animation: {
        type: 4,
        styles: {
          type: 6,
          styles: {
            opacity: 1,
            transform: "scale(1, 1)"
          },
          offset: null
        },
        timings: "120ms cubic-bezier(0, 0, 0.2, 1)"
      },
      options: null
    }, {
      type: 1,
      expr: "* => void",
      animation: {
        type: 4,
        styles: {
          type: 6,
          styles: {
            opacity: 0
          },
          offset: null
        },
        timings: "100ms linear"
      },
      options: null
    }],
    options: {}
  }
};
export {
  MAT_SELECT_CONFIG,
  MAT_SELECT_SCROLL_STRATEGY,
  MAT_SELECT_SCROLL_STRATEGY_PROVIDER,
  MAT_SELECT_SCROLL_STRATEGY_PROVIDER_FACTORY,
  MAT_SELECT_TRIGGER,
  MatError,
  MatFormField,
  MatHint,
  MatLabel,
  MatOptgroup,
  MatOption,
  MatPrefix,
  MatSelect,
  MatSelectChange,
  MatSelectModule,
  MatSelectTrigger,
  MatSuffix,
  matSelectAnimations
};
//# sourceMappingURL=@angular_material_select.js.map
