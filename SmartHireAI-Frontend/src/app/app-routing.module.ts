import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { UploadResumeComponent } from './components/upload-resume/upload-resume.component';
import { JobsComponent } from './components/jobs/jobs.component';
import { CandidateRankingComponent } from './components/candidate-ranking/candidate-ranking.component';
import { AnalysisComponent } from './components/analysis/analysis.component';

const routes: Routes = [
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'upload', component: UploadResumeComponent },
  { path: 'jobs', component: JobsComponent },
  { path: 'analysis', component: AnalysisComponent },
  { path: 'ranking', component: CandidateRankingComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
