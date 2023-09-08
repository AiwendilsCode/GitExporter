# GitExporter
Program for exporting repository commits to directories.

## Usage
GitExporter.exe <GIT_REPOSITORY_DIR> <OUTPUT_DIR>

## What it does
For every commit creates new directory with name sha1 of that commit. This directory will contain state of repository at that particular commit. Adds commitInformations.txt file in directory in which is stored output of command git show --no-patch \<SHA1\>.
