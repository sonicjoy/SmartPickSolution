#!/usr/bin/env groovy

pipeline {
  agent none
  stages {
    stage('git checkout') {
      agent { label 'master' }
      steps {
        checkout scm
      }
    }
    stage('test') {
      agent { label 'master' }
      steps {
        sh 'dotnet test'
      }
    }
    stage('deploy staging') {
      agent { label 'stg-tote-master' }
      when {
        branch 'master'
      }
      steps {
        sh 'ssh -i /var/lib/jenkins/.ssh/deployer dotnet@smart-pick-01.tote-slave.stg.ovh.local deploy.sh'
      }
    }
    stage('deploy production ovh') {
      agent { label 'prd-ovh' }
      when {
        branch 'release'
      }
      steps {
        sh 'ssh -i /var/lib/jenkins/.ssh/deployer dotnet@smart-pick-01.prd.ovh.local deploy.sh'
        sh 'ssh -i /var/lib/jenkins/.ssh/deployer dotnet@smart-pick-02.prd.ovh.local deploy.sh'
      }
    }
    stage('deploy production tw') {
      agent { label 'prd-tw' }
      when {
        branch 'release'
      }
      steps {
        sh 'ssh -i /var/lib/jenkins/.ssh/deployer dotnet@smart-pick-01.prd.tw.local deploy.sh'
        sh 'ssh -i /var/lib/jenkins/.ssh/deployer dotnet@smart-pick-02.prd.tw.local deploy.sh'
      }
    }
    stage('deploy production ovh-ca') {
      agent { label 'prd-ovh-ca' }
      when {
        branch 'release'
      }
      steps {
        sh 'ssh -i /var/lib/jenkins/.ssh/deployer dotnet@smart-pick-01.prd.ovh-ca.local deploy.sh'
        sh 'ssh -i /var/lib/jenkins/.ssh/deployer dotnet@smart-pick-02.prd.ovh-ca.local deploy.sh'
      }
    }
  }
  post {
    success {
      slackSend channel: '#ci-dotnet', color: '#00FF00', message: "SUCCESSFUL: Job '${env.JOB_NAME} [${env.BUILD_NUMBER}]' (${env.BUILD_URL})"
    }
    failure {
      slackSend channel: '#ci-dotnet', color: '#FF0000', message: "FAILED: Job '${env.JOB_NAME} [${env.BUILD_NUMBER}]' (${env.BUILD_URL})"
    }
  }
}
