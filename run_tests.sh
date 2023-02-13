#!/bin/bash
/Applications/Unity/Hub/Editor/2021.3.17f1/Unity.app/Contents/MacOS/Unity \
  -runTests \
  -batchmode \
  -projectPath ~/scripted_rollaball \
  -testResults ./test-results.xml \
  -testPlatform PlayMode
