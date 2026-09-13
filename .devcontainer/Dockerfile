# Fedora
FROM fedora:41

ENV LANG=C.UTF-8
ENV LC_ALL=C.UTF-8

RUN dnf -y update && dnf clean all

# 🔧 TOOLCHAIN (based on build-containers)
RUN dnf install -y \
    # Tools
    git \
    wget \
    curl \
    unzip \
    which \
    python3 \
    python3-pip \
    python3-devel \
    python3-setuptools \
    python3-wheel \
    python3-numpy \
    python3-scipy \
    python3-scikit-learn \
    python3-aiohttp \
    python3-pandas \
    redhat-rpm-config \
    scons \
    #godot build deps
    gcc \
    gcc-c++ \
    make \
    clang \
    cmake \
    ninja-build \
    pkgconf-pkg-config \
    # runtime libs
    libX11 \
    libXcursor \
    libXrandr \
    libXinerama \
    libXi \
    libXext \
    libXrender \
    mesa-libGL \
    alsa-lib \
    pulseaudio-libs \
    fontconfig \
    freetype \
    libpng \
    libogg \
    libvorbis \
    # 🔥 headers (IMPORTANT)
    libX11-devel \
    libXcursor-devel \
    libXrandr-devel \
    libXinerama-devel \
    libXi-devel \
    libXext-devel \
    libXrender-devel \
    mesa-libGL-devel \
    alsa-lib-devel \
    # 🔥 build deps
    openssl-devel \
    zlib-devel \
    icu \
    # 🔥 Vulkan (very important)
    vulkan-loader \
    vulkan-loader-devel \
    # .NET 10 LTS
    dotnet-sdk-10.0 \
    # context-mode deps
    nodejs \
    npm \
    && dnf clean all

# Install OpenCode
RUN curl -fsSL https://opencode.ai/install | bash
ENV PATH="/root/.opencode/bin:$PATH"

# Instalar context-mode
RUN git clone https://github.com/mksglu/context-mode.git /opt/context-mode \
    && cd /opt/context-mode \
    && npm install \
    && npm install -g .

# 🔥 (OPTIONAL but powerful)
# Mono helps with compatibility with some Godot tools
RUN dnf install -y mono-complete || true

# =========================
# GODOT .NET (already validated)
# =========================
ENV GODOT_VERSION=4.6.2
ENV GODOT_FILE_ZIP=Godot_v${GODOT_VERSION}-stable_mono_linux_x86_64
ENV GODOT_FILE=Godot_v${GODOT_VERSION}-stable_mono_linux.x86_64

RUN wget https://github.com/godotengine/godot/releases/download/${GODOT_VERSION}-stable/${GODOT_FILE_ZIP}.zip && \
    unzip ${GODOT_FILE_ZIP}.zip && \
    mv ${GODOT_FILE_ZIP}/* /usr/local/bin/ && \
    rm -rf ${GODOT_FILE_ZIP} ${GODOT_FILE_ZIP}.zip && \
    mv /usr/local/bin/${GODOT_FILE} /usr/local/bin/godot && \
    chmod +x /usr/local/bin/godot

ENV PATH="/usr/local/bin:$PATH"

# =========================
# VALIDATION
# =========================
RUN godot --version
RUN dotnet --version

WORKDIR /workspace

# Automatic C# test
COPY tests/test_csharp.sh /test_csharp.sh
RUN sed -i 's/\r$//' /test_csharp.sh && chmod +x /test_csharp.sh && /test_csharp.sh