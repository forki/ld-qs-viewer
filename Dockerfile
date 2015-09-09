FROM nice/ld-docker-build 

MAINTAINER James Kirk <james.kirk84@gmail.com>

COPY . /etc/ld-viewer/

RUN /etc/ld-viewer/build.sh

CMD mono /etc/ld-viewer/bin/viewer/viewer.exe

EXPOSE 8083
