<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:template match="/">
    <html>
    <body>
    <!--
        This is an XSLT template file. Fill in this area with the
        XSL elements which will transform your XML to XHTML.
    -->
      <xsl:for-each select="dsSchema/tblCharacters">
        <a href="#">
          <xsl:attribute name ="onclick">javascript:Character_onclick('<xsl:value-of select="Character_Caption"/>');</xsl:attribute>
          <xsl:value-of select="Character_Caption"/>
        </a>
      </xsl:for-each>
      <a class="all" href="#" onclick="javascript:btnShowAll_onclick();" id="lblShowAll">Show All</a>
      <div class="clr"></div>
    </body>
    </html>
</xsl:template>

</xsl:stylesheet> 

