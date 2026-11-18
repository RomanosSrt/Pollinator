import { Component, computed, input } from '@angular/core';

import { Metric } from '../../../core/models/metric.model';
import { METRIC_SCALES } from '../metric-scales';

// Map legend: the bands of the chosen metric with their colours and value ranges.
@Component({
  selector: 'app-band-legend',
  templateUrl: './band-legend.component.html',
})
export class BandLegendComponent {
  readonly metric = input.required<Metric>();

  private readonly scale = computed(() => METRIC_SCALES[this.metric().key]);

  // Weather bands are code groups, so showing code ranges would not help.
  private readonly showRanges = computed(() => this.metric().key !== 'wmoCode');

  protected readonly rows = computed(() =>
    this.scale().bands.map((band, i, bands) => {
      const lower = i > 0 ? bands[i - 1].max : undefined;
      let range = '';
      if (this.showRanges()) {
        if (lower === undefined) range = `< ${band.max}`;
        else if (band.max === Infinity) range = `≥ ${lower}`;
        else range = `${lower}–${band.max}`;
      }
      return { label: band.label, color: band.color, range };
    }),
  );

  // Explains the red border, when the metric has one.
  protected readonly alarmNote = computed(() => {
    const from = this.scale().alarmFrom;
    return from === undefined ? '' : `Red border: ${this.scale().bands[from].label} or worse`;
  });
}
