#!/bin/sh

set -eu
set -o pipefail

cd `dirname $0`

FSIARGS=""
OS=${OS:-"unknown"}
if [[ "$OS" != "Windows_NT" ]]
then
  FSIARGS="--fsiargs -d:MONO"
fi

mono .paket/paket.bootstrapper.exe

if [[ "$OS" != "Windows_NT" ]] &&
       [ ! -e ~/.config/.mono/certs ]
then
  mozroots --import --sync --quiet
fi

mono .paket/paket.exe restore

[ ! -e build.fsx ] && run .paket/paket.exe update
[ ! -e build.fsx ] && run packages/FAKE/tools/FAKE.exe init.fsx
mono packages/FAKE/tools/FAKE.exe "$@" $FSIARGS build.fsx

