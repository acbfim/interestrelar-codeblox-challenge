import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'usCurrency'
})
export class UsCurrencyPipe implements PipeTransform {
  transform(value: number): string {
    return Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(value);
  }
}
