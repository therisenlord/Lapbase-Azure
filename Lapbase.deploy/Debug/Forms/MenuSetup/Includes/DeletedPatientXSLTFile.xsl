<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt">
  <xsl:template match="/">
    <html>
      <body>

        <table style="width:97%" cellpadding = "0" cellspacing = "0" border="0" id ="tblDeletedPatient" class="testNameTable">
          <xsl:for-each select ="/dsSchema/tblDeletedPatient">
            <tr >
              <xsl:if test='position() mod 2 = 1'>
                <xsl:attribute name="class">row01</xsl:attribute>
              </xsl:if>
              <xsl:if test='position() mod 2 != 1'>
                <xsl:attribute name="class">row02</xsl:attribute>
              </xsl:if>
              <td style="width :10%" >
                <xsl:value-of select="Patient_CustomId"/>
              </td>
              <td style="width :20%" >
                <xsl:value-of select="FullName"/>
              </td>
              <td style="width :20%" >
                <xsl:value-of select="BirthDate"/>
              </td>
              <td style="width :20%" >
                <xsl:value-of select="DeletedByUser"/>
              </td>
              <td style="width :20%" >
                <xsl:value-of select="DateDeleted"/>
              </td>
              <td style="width :5%;color:blue">
                <xsl:attribute name="align">right</xsl:attribute>
                <xsl:attribute name ="onclick">
                  javascript:ActivatePatient_onclick(<xsl:value-of select="PatientId"/>);
                </xsl:attribute>
                <xsl:attribute name="onmouseover">
                  javascript:this.style.cursor='pointer';
                </xsl:attribute>
                <xsl:attribute name="onmouseout">
                  javascript:this.style.cursor='';
                </xsl:attribute>
                <u>Activate</u>
              </td>
            </tr>
          </xsl:for-each>
        </table>
      </body>
    </html>
  </xsl:template>

</xsl:stylesheet>

