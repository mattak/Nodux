#!/bin/sh

rm -rf doc/_site/{api,articles,fonts,styles,favicon.ico}
rm -rf doc/obj/
docfx doc/docfx.json $@
