import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, NgZone } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Auth } from '../../../core/services/auth';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  loginForm: FormGroup;
  requestOtpForm: FormGroup;
  verifyOtpForm: FormGroup;
  resetPasswordForm: FormGroup;

  currentStep = 0;
  resetIdentifier = '';

  isLoading = false;
  errorMessage = '';
  successMessage = '';
  showPassword = false;

  constructor(
    private fb: FormBuilder,
    private authService: Auth,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private zone: NgZone,
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
    });

    this.requestOtpForm = this.fb.group({
      emailOrPhone: ['', [Validators.required]],
    });

    this.verifyOtpForm = this.fb.group({
      otpCode: ['', [Validators.required]],
    });

    this.resetPasswordForm = this.fb.group({
      newPassword: ['', [Validators.required]],
    });
  }

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }

  setStep(step: number) {
    this.currentStep = step;
    this.errorMessage = '';
    this.successMessage = '';
  }

  onSubmit() {
    if (this.loginForm.invalid) return;

    this.isLoading = true;
    this.errorMessage = '';

    this.authService.login(this.loginForm.value).subscribe({
      next: (response) => {
        this.zone.run(() => {
          localStorage.setItem('jwt_token', response.token);
          localStorage.setItem('user_role', response.userRole);
          localStorage.setItem('global_id', response.globalId);

          if (response.userRole === 'Admin') {
            this.router.navigate(['/dashboard']);
          } else {
            this.router.navigate(['/employee']);
          }
          this.cdr.detectChanges();
        });
      },
      error: (err) => {
        this.zone.run(() => {
          this.isLoading = false;
          this.errorMessage = err.error?.message || 'Invalid email or password. Please try again.';
          this.cdr.detectChanges();
        });
      },
    });
  }

  onRequestOtpSubmit() {
    if (this.requestOtpForm.invalid) return;

    this.isLoading = true;
    this.errorMessage = '';
    this.resetIdentifier = this.requestOtpForm.value.emailOrPhone;

    this.authService.requestOtp({ emailOrPhone: this.resetIdentifier }).subscribe({
      next: (res) => {
        this.zone.run(() => {
          this.isLoading = false;
          console.log('SIMULATED OTP RECEIVED:', res.otpCode);
          this.setStep(2);
          this.cdr.detectChanges();
        });
      },
      error: (err) => {
        this.zone.run(() => {
          this.isLoading = false;
          this.errorMessage = err.error?.message || 'User not found.';
          this.cdr.detectChanges();
        });
      },
    });
  }

  onVerifyOtpSubmit() {
    if (this.verifyOtpForm.invalid) return;

    this.isLoading = true;
    this.errorMessage = '';

    const payload = {
      emailOrPhone: this.resetIdentifier,
      otpCode: this.verifyOtpForm.value.otpCode,
    };

    this.authService.verifyOtp(payload).subscribe({
      next: () => {
        this.zone.run(() => {
          this.isLoading = false;
          this.setStep(3);
          this.cdr.detectChanges();
        });
      },
      error: (err) => {
        this.zone.run(() => {
          this.isLoading = false;
          this.errorMessage = err.error?.message || 'Invalid or expired OTP.';
          this.cdr.detectChanges();
        });
      },
    });
  }

  onResetPasswordSubmit() {
    if (this.resetPasswordForm.invalid) return;

    this.isLoading = true;
    this.errorMessage = '';

    const payload = {
      emailOrPhone: this.resetIdentifier,
      newPassword: this.resetPasswordForm.value.newPassword,
    };

    this.authService.resetPassword(payload).subscribe({
      next: () => {
        this.zone.run(() => {
          this.isLoading = false;
          this.setStep(0);
          this.successMessage = 'Password reset successful. Please log in with your new password.';
          this.loginForm.reset();
          this.cdr.detectChanges();
        });
      },
      error: (err) => {
        this.zone.run(() => {
          this.isLoading = false;
          this.errorMessage = err.error?.message || 'Failed to reset password.';
          this.cdr.detectChanges();
        });
      },
    });
  }
}
