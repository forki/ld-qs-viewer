FROM oarfish/alpine-fsharp

MAINTAINER James Kirk <james.kirk84@gmail.com>

COPY . /etc/ld-viewer/

RUN /etc/ld-viewer/build.sh &&\
    find /etc/ld-viewer/ -type d | grep -v 'bin' | xargs -i rm -rf {}

CMD cd /etc/ld-viewer/ && fsharpi bin/viewer/RunServer.fsx

EXPOSE 8083
