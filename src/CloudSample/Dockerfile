FROM mono:3.12

ENV KRE_VERSION 1.0.0-beta2
ENV KRE_USER_HOME /opt/kre

RUN apt-get -qq update && apt-get -qqy install unzip apt-utils

RUN curl -sSL https://raw.githubusercontent.com/aspnet/Home/release/kvminstall.sh | KVM_BRANCH=v$KRE_VERSION sh

RUN bash -c "source $KRE_USER_HOME/kvm/kvm.sh \
&& kvm install $KRE_VERSION -a default \
&& kvm upgrade \
&& kvm alias default | xargs -i ln -s $KRE_USER_HOME/packages/{} $KRE_USER_HOME/packages/default"

RUN apt-get -qq update

RUN apt-get -qq install mono-complete

RUN apt-get -qq update

# Install libuv for Kestrel from source code (binary is not in wheezy and one in jessie is still too old)
RUN apt-get -qqy install autoconf automake build-essential libtool gyp libssl-dev python apt-utils

RUN apt-get -qq update

RUN curl -sL https://deb.nodesource.com/setup | bash -
RUN apt-get -qqy install nodejs && npm install -g grunt-cli

RUN apt-get -qq update

RUN LIBUV_VERSION=1.0.0-rc2 \
&& curl -sSL https://github.com/joyent/libuv/archive/v${LIBUV_VERSION}.tar.gz | tar zxfv - -C /usr/local/src \
&& cd /usr/local/src/libuv-$LIBUV_VERSION \
&& sh autogen.sh && ./configure && make && make install \
&& rm -rf /usr/local/src/libuv-$LIBUV_VERSION \
&& ldconfig

RUN apt-get -qq update

ENV PATH $PATH:$KRE_USER_HOME/packages/default/bin

RUN apt-get -qq upgrade

COPY . /app

WORKDIR /app/

RUN ["kpm", "restore"]

EXPOSE 5022

ENTRYPOINT ["k", "kestrel"]
