import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ToastService {

  constructor() {
    this.createToastContainer();
   }
  private createToastContainer() {
    if (!document.getElementById('toast-container')) {
      const container = document.createElement('div');
      container.id = 'toast-container';
      container.className = 'toast toast-bottom toast-end';
      document.body.appendChild(container);
    }
  }

  private createToastElement(message: string, alert: string, duration = 5000) {
    const toast = document.getElementById('toast-container');
    if(!toast) return;

    const toastElement = document.createElement('div');
    toastElement.classList.add('alert', alert, 'shadow-lg');
    toastElement.innerHTML = `
    <span>${message}</span>
    <button class="btn btn-sm btn-ghost ml-4">X</button>
    `;
    toastElement.querySelector('button')?.addEventListener('click', () => {
      toast.removeChild(toastElement);
    });
    toast.append(toastElement);

    setTimeout(() => {
      if (toast.contains(toastElement)) {
        toast.removeChild(toastElement);
      }
    }, duration);
  }

  success(message: string, duration?: number) {
    this.createToastElement(message, 'alert-success', duration);
  }

  error(message: string, duration?: number) {
    this.createToastElement(message, 'alert-error', duration);
  }

  warning(message: string, duration?: number) {
    this.createToastElement(message, 'alert-warning', duration);
  }
  
  info(message: string, duration?: number) {
    this.createToastElement(message, 'alert-info', duration);
  }
}
