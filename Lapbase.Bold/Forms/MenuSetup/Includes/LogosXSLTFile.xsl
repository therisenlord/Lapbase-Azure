<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt">

<xsl:template match="/">
    <html>
    <body>
      
      <table style="width:97%" cellpadding = "0" cellspacing = "0" border="0" id ="tblLogoXSLT" class="testNameTable">
        <xsl:for-each select ="/dsSchema/tblLogo">
        <tr >
          <xsl:if test='position() mod 2 = 1'>
            <xsl:attribute name="class">row01</xsl:attribute>
          </xsl:if>
          <xsl:if test='position() mod 2 != 1'>
            <xsl:attribute name="class">row02</xsl:attribute>
          </xsl:if>
          <td style="width :92%"  >
            <xsl:attribute name ="onclick">
              javascript:tblLogoXSLT_onclick('<xsl:value-of select="LogoId"/>');
            </xsl:attribute>
            <xsl:value-of select="LogoName"/>
          </td>
          <td style="width :5%"  >
            <xsl:attribute name="align">right</xsl:attribute>
            <xsl:attribute name ="onclick">
              javascript:DeleteLogo_onclick(<xsl:value-of select="LogoId"/>);
            </xsl:attribute>
            <xsl:attribute name="onmouseover">
              javascript:this.style.cursor='pointer';
            </xsl:attribute>
            <xsl:attribute name="onmouseout">
              javascript:this.style.cursor='';
            </xsl:attribute>
            <u>Delete</u>
          </td>
        </tr>
        </xsl:for-each>
      </table>
    </body>
    </html>
</xsl:template>

</xsl:stylesheet> 

