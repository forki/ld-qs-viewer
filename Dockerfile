FROM nhsevidence/alpine-fsharp:ee06745b8aa982314a57820129bc59c9d7b383d4

MAINTAINER James Kirk <james.kirk84@gmail.com>

COPY . /etc/ld-viewer/

RUN /etc/ld-viewer/build.sh &&\
    find /etc/ld-viewer/ -type d | grep -v 'bin' | xargs -i rm -rf {}

CMD cd /etc/ld-viewer/ && fsharpi bin/viewer/RunServer.fsx

EXPOSE 8083
