#!/bin/bash
URL_TO_TEST=$1

delaySecs=10
code=1

echo "Starting smoke tests for $URL_TO_TEST"
for i in {0..2} 
do
    sleep $delaySecs
    echo "Running smoke test attempt $i"
    curl -I -XGET --silent -v "$URL_TO_TEST" | grep 200
    code=$?
    if [ $code -eq 0 ]; then
        break
    fi
    echo "Smoke test failed, retrying..."
done

echo "finished smoke tests for $URL_TO_TEST"
if [ $code -ne 0 ]; then
    exit $code
fi