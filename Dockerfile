FROM ocelotuproar/docker-alpine-fsharp:4.0

MAINTAINER James Kirk <james.kirk84@gmail.com>

RUN mkdir /discoverytool

# Keep package management separate from code
ADD paket.dependencies paket.lock .paket/ /discoverytool/
ADD .paket/ /discoverytool/.paket/

WORKDIR /discoverytool

RUN mono .paket/paket.bootstrapper.exe && mono .paket/paket.exe install

ADD RELEASE_NOTES.md build.sh populateAppSettings.sh build.fsx viewer.sln /discoverytool/
ADD src /discoverytool/src
ADD tests /discoverytool/tests

RUN /discoverytool/build.sh &&\
    cd /discoverytool && \
    ls -a . | grep -v "bin" | xargs -i rm -rf {}

CMD populateAppSettings.sh && mono bin/viewer/viewer.exe $SERVER_MODE

EXPOSE 8083
	