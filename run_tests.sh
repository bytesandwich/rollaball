#!/bin/bash
VERSION="2021.3.19f1-arm64"
"/Applications/Unity/Hub/Editor/${VERSION}/Unity.app/Contents/MacOS/Unity" \
  -runTests \
  -batchmode \
  -projectPath ~/scripted_rollaball \
  -testResults ./test-results.xml \
  -testPlatform PlayMode
