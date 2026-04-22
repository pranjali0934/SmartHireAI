import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-upload-resume',
  templateUrl: './upload-resume.component.html',
  styleUrls: ['./upload-resume.component.scss']
})
export class UploadResumeComponent {
  form: FormGroup;
  file: File | null = null;
  uploadedResponse: { extractedText?: string; fileName: string } | null = null;
  error = '';
  loading = false;

  constructor(private fb: FormBuilder, private api: ApiService) {
    this.form = this.fb.group({
      candidateName: ['', Validators.required],
      candidateEmail: ['', [Validators.required, Validators.email]]
    });
  }

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files?.length) {
      const f = input.files[0];
      const ext = f.name.toLowerCase().split('.').pop();
      if (['pdf', 'docx', 'doc', 'txt'].includes(ext || '')) {
        this.file = f;
        this.error = '';
      } else {
        this.file = null;
        this.error = 'Only PDF, DOCX, and TXT files are supported.';
      }
    }
  }

  submit() {
    if (!this.form.valid || !this.file) {
      this.error = 'Please fill all fields and select a file.';
      return;
    }
    this.loading = true;
    this.error = '';
    this.uploadedResponse = null;
    this.api.uploadResume(
      this.file,
      this.form.value.candidateName,
      this.form.value.candidateEmail
    ).subscribe({
      next: (res) => {
        this.uploadedResponse = { extractedText: res.extractedText, fileName: res.fileName };
        this.loading = false;
        this.form.reset();
        this.file = null;
      },
      error: (err) => {
        this.error = err.error?.message || err.statusText || 'Upload failed.';
        this.loading = false;
      }
    });
  }
}
