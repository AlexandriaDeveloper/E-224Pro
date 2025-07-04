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
} from "./chunk-MFYQHCGF.js";
import "./chunk-DSOQ457I.js";
import {
  MatOptgroup,
  MatOption
} from "./chunk-LNF4RSYZ.js";
import "./chunk-ERFID2QG.js";
import "./chunk-OVBP52S6.js";
import "./chunk-HU2JCZH5.js";
import "./chunk-Z5GCHW2Q.js";
import "./chunk-OIZAD6NR.js";
import {
  MatError,
  MatFormField,
  MatHint,
  MatLabel,
  MatPrefix,
  MatSuffix
} from "./chunk-LRTPQZS3.js";
import "./chunk-BSANYXFT.js";
import "./chunk-6JES4S7P.js";
import "./chunk-SZS4RJEH.js";
import "./chunk-UDU42JBG.js";
import "./chunk-FRSKEUM3.js";
import "./chunk-AUPMVO2U.js";
import "./chunk-I64KL53I.js";
import "./chunk-GITX2MNP.js";
import "./chunk-2PY7ANQC.js";
import "./chunk-UU5Z7QKS.js";
import "./chunk-X3P5GA7C.js";
import "./chunk-O6QQYGRN.js";
import "./chunk-2WIRX57M.js";
import "./chunk-TV5GMRHO.js";
import "./chunk-M3HR6BUY.js";
import "./chunk-65RJ5ZZ2.js";
import "./chunk-I2T7AJLJ.js";
import "./chunk-BCS43ZSB.js";
import "./chunk-2CFPYFPO.js";
import "./chunk-UEDCDFWL.js";
import "./chunk-YNRXC4MO.js";
import "./chunk-WTR5NLJH.js";
import "./chunk-GVPTSQHO.js";
import "./chunk-BXKBUVMM.js";
import "./chunk-JINMNLB2.js";
import "./chunk-C6ZQOQFD.js";
import "./chunk-2C44WUKA.js";

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
