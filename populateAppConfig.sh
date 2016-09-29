#!/bin/sh
# Script to populate the app.config with environment variables
sed -i "s/LOGGING_ENVIRONMENT/$LOGGING_ENVIRONMENT/" bin/viewer/viewer.exe.config

