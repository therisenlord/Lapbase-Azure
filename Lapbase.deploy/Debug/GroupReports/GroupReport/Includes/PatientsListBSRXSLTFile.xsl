<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt">

<xsl:template match="/">
    <html>
    <body>
		<!--<div class="visitDisplayScroll" >-->
			  <table style="width:895px" cellpadding = "0" cellspacing = "0" border="0" class="testNameTable">
				<xsl:for-each select="dsSchema/tblPatients">
				  <tr>
					  <td>
						  <xsl:if test='position() mod 2 = 1'>
							  <xsl:attribute name="class">row01</xsl:attribute>
						  </xsl:if>
						  <xsl:if test='position() mod 2 != 1'>
							  <xsl:attribute name="class">row02</xsl:attribute>
						  </xsl:if>
						  <table style="width:90%" cellpadding = "0" cellspacing = "0" border="0">
							  <tr>
								  <td style="width:8%">
                    <xsl:choose>
                      <xsl:when test ="string-length(Patient_CustomId) > 0">
                        <xsl:value-of select="Patient_CustomId"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="PatientID"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </td>
                  <td style="width:17%">
                    <xsl:value-of select="Surname"/>
                  </td>
                  <td style="width:11%">
                    <xsl:value-of select="Firstname"/>
                  </td>
                  <td style="width:3%">
                    <input type="checkbox">
                      <xsl:call-template name ="fnRadioCheck">
                        <xsl:with-param name="form" select ="'reg1'"/>
                        <xsl:with-param name="year" select ="1"/>
                        <xsl:with-param name="value" select ="Reg1Y1"/>
                        <xsl:with-param name="patientid" select ="PatientID"/>
                      </xsl:call-template>
                    </input>
                  </td>
                  <td style="width:4%">
                    <xsl:call-template name ="fnClickable">
                      <xsl:with-param name="form" select ="'reg1'"/>
                      <xsl:with-param name="year" select ="1"/>
                      <xsl:with-param name="value" select ="Reg1Y1"/>
                      <xsl:with-param name="patientid" select ="PatientID"/>
                      <xsl:with-param name="admitid" select ="baselineOperationID"/>
                    </xsl:call-template>
                    <xsl:value-of select="Reg1Y1"/>
								  </td>
                  <td style="width:3%">
                    <input type="checkbox">
                      <xsl:call-template name ="fnRadioCheck">
                        <xsl:with-param name="form" select ="'reg2'"/>
                        <xsl:with-param name="year" select ="1"/>
                        <xsl:with-param name="value" select ="Reg2Y1"/>
                        <xsl:with-param name="patientid" select ="PatientID"/>
                      </xsl:call-template>
                    </input>
                  </td>
                  <td style="width:4%">
                    <xsl:call-template name ="fnClickable">
                      <xsl:with-param name="form" select ="'reg2'"/>
                      <xsl:with-param name="year" select ="1"/>
                      <xsl:with-param name="value" select ="Reg2Y1"/>
                      <xsl:with-param name="patientid" select ="PatientID"/>
                      <xsl:with-param name="admitid" select ="baselineOperationID"/>
                    </xsl:call-template>
                    <xsl:value-of select="Reg2Y1"/>
								  </td>
                  <td style="width:3%">
                    <input type="checkbox">
                      <xsl:call-template name ="fnRadioCheck">
                        <xsl:with-param name="form" select ="'reg2'"/>
                        <xsl:with-param name="year" select ="2"/>
                        <xsl:with-param name="value" select ="Reg2Y2"/>
                        <xsl:with-param name="patientid" select ="PatientID"/>
                      </xsl:call-template>
                    </input>
                  </td>
								  <td style="width:4%">
                    <xsl:call-template name ="fnClickable">
                      <xsl:with-param name="form" select ="'reg2'"/>
                      <xsl:with-param name="year" select ="2"/>
                      <xsl:with-param name="value" select ="Reg2Y2"/>
                      <xsl:with-param name="patientid" select ="PatientID"/>
                      <xsl:with-param name="admitid" select ="baselineOperationID"/>
                    </xsl:call-template>     
                    <xsl:value-of select="Reg2Y2"/>
								  </td>
                  <td style="width:3%">
                    <input type="checkbox">
                      <xsl:call-template name ="fnRadioCheck">
                        <xsl:with-param name="form" select ="'reg2'"/>
                        <xsl:with-param name="year" select ="3"/>
                        <xsl:with-param name="value" select ="Reg2Y3"/>
                        <xsl:with-param name="patientid" select ="PatientID"/>
                      </xsl:call-template>
                    </input>
                  </td>
                  <td style="width:4%">
                    <xsl:call-template name ="fnClickable">
                      <xsl:with-param name="form" select ="'reg2'"/>
                      <xsl:with-param name="year" select ="3"/>
                      <xsl:with-param name="value" select ="Reg2Y3"/>
                      <xsl:with-param name="patientid" select ="PatientID"/>
                      <xsl:with-param name="admitid" select ="baselineOperationID"/>
                    </xsl:call-template>
                    <xsl:value-of select="Reg2Y3"/>
								  </td>
                  <td style="width:3%">
                    <input type="checkbox">
                      <xsl:call-template name ="fnRadioCheck">
                        <xsl:with-param name="form" select ="'reg2'"/>
                        <xsl:with-param name="year" select ="4"/>
                        <xsl:with-param name="value" select ="Reg2Y4"/>
                        <xsl:with-param name="patientid" select ="PatientID"/>
                      </xsl:call-template>
                    </input>
                  </td>
                  <td style="width:4%">
                    <xsl:call-template name ="fnClickable">
                      <xsl:with-param name="form" select ="'reg2'"/>
                      <xsl:with-param name="year" select ="4"/>
                      <xsl:with-param name="value" select ="Reg2Y4"/>
                      <xsl:with-param name="patientid" select ="PatientID"/>
                      <xsl:with-param name="admitid" select ="baselineOperationID"/>
                    </xsl:call-template> 
                    <xsl:value-of select="Reg2Y4"/>
                  </td>
                  <td style="width:3%">
                    <input type="checkbox">
                      <xsl:call-template name ="fnRadioCheck">
                        <xsl:with-param name="form" select ="'reg2'"/>
                        <xsl:with-param name="year" select ="5"/>
                        <xsl:with-param name="value" select ="Reg2Y5"/>
                        <xsl:with-param name="patientid" select ="PatientID"/>
                      </xsl:call-template>
                    </input>
                  </td>
								  <td style="width:4%">
                    <xsl:call-template name ="fnClickable">
                      <xsl:with-param name="form" select ="'reg2'"/>
                      <xsl:with-param name="year" select ="5"/>
                      <xsl:with-param name="value" select ="Reg2Y5"/>
                      <xsl:with-param name="patientid" select ="PatientID"/>
                      <xsl:with-param name="admitid" select ="baselineOperationID"/>
                    </xsl:call-template>
                    <xsl:value-of select="Reg2Y5"/>
                  </td>
                  <td style="width:3%">
                    <input type="checkbox">
                      <xsl:call-template name ="fnRadioCheck">
                        <xsl:with-param name="form" select ="'reg2'"/>
                        <xsl:with-param name="year" select ="6"/>
                        <xsl:with-param name="value" select ="Reg2Y6"/>
                        <xsl:with-param name="patientid" select ="PatientID"/>
                      </xsl:call-template>
                    </input>
                  </td>
                  <td style="width:4%">
                    <xsl:call-template name ="fnClickable">
                      <xsl:with-param name="form" select ="'reg2'"/>
                      <xsl:with-param name="year" select ="6"/>
                      <xsl:with-param name="value" select ="Reg2Y6"/>
                      <xsl:with-param name="patientid" select ="PatientID"/>
                      <xsl:with-param name="admitid" select ="baselineOperationID"/>
                    </xsl:call-template>       
                    <xsl:value-of select="Reg2Y6"/>
                  </td>
                  <td style="width:3%">
                    <input type="checkbox">
                      <xsl:call-template name ="fnRadioCheck">
                        <xsl:with-param name="form" select ="'reg2'"/>
                        <xsl:with-param name="year" select ="7"/>
                        <xsl:with-param name="value" select ="Reg2Y7"/>
                        <xsl:with-param name="patientid" select ="PatientID"/>
                      </xsl:call-template>
                    </input>
                  </td>
                  <td style="width:4%">
                    <xsl:call-template name ="fnClickable">
                      <xsl:with-param name="form" select ="'reg2'"/>
                      <xsl:with-param name="year" select ="7"/>
                      <xsl:with-param name="value" select ="Reg2Y7"/>
                      <xsl:with-param name="patientid" select ="PatientID"/>
                      <xsl:with-param name="admitid" select ="baselineOperationID"/>
                    </xsl:call-template>
                    <xsl:value-of select="Reg2Y7"/>
                  </td>
                  <td style="width:3%">
                    <input type="checkbox">
                      <xsl:call-template name ="fnRadioCheck">
                        <xsl:with-param name="form" select ="'reg2'"/>
                        <xsl:with-param name="year" select ="8"/>
                        <xsl:with-param name="value" select ="Reg2Y8"/>
                        <xsl:with-param name="patientid" select ="PatientID"/>
                      </xsl:call-template>
                    </input>
                  </td>
                  <td style="width:4%">
                    <xsl:call-template name ="fnClickable">
                      <xsl:with-param name="form" select ="'reg2'"/>
                      <xsl:with-param name="year" select ="8"/>
                      <xsl:with-param name="value" select ="Reg2Y8"/>
                      <xsl:with-param name="patientid" select ="PatientID"/>
                      <xsl:with-param name="admitid" select ="baselineOperationID"/>
                    </xsl:call-template>  
                    <xsl:value-of select="Reg2Y8"/>
                  </td>
							  </tr>
						  </table>
					  </td>
				  </tr>
				</xsl:for-each>
			  </table>
		<!--</div>-->
    </body>
    </html>
</xsl:template>

<xsl:template name = "fnRadioCheck">
  <xsl:param name ="form" ></xsl:param>
  <xsl:param name ="year" ></xsl:param>
  <xsl:param name ="value" ></xsl:param>
  <xsl:param name ="patientid" ></xsl:param>

  <xsl:attribute name="id">
    <xsl:value-of select="concat($patientid,'-',$form,'-',$year)"/>
  </xsl:attribute>

  <xsl:attribute name="onclick">
    javascript:BSFormCheckbox_onclick('<xsl:value-of select="$form"/>', '<xsl:value-of select="$year"/>', <xsl:value-of select="$patientid"/>);
  </xsl:attribute>
  
  <xsl:choose>
    <xsl:when test ="$value = 'Due'">
      <xsl:attribute name="style">
        visibility:display
      </xsl:attribute>
    </xsl:when>
    <xsl:when test ="$value = 'Ok'">
      <xsl:attribute name="style">
        visibility:hidden
      </xsl:attribute>
    </xsl:when>
    <xsl:otherwise>
      <xsl:attribute name="style">
        visibility:hidden
      </xsl:attribute>
    </xsl:otherwise>
  </xsl:choose>
</xsl:template>
  
  
  
  
  
<xsl:template name = "fnClickable">
  <xsl:param name ="form" ></xsl:param>
  <xsl:param name ="year" ></xsl:param>
  <xsl:param name ="value" ></xsl:param>
  <xsl:param name ="patientid" ></xsl:param>
  <xsl:param name ="admitid" ></xsl:param>
  <xsl:choose>
    <xsl:when test ="$value = 'Due'">
      <xsl:attribute name="style">
        color:green;width:63px
      </xsl:attribute>
      <xsl:attribute name="onclick">
        javascript:BSForm_onclick('<xsl:value-of select="$form"/>', '<xsl:value-of select="$year"/>', '<xsl:value-of select="$value"/>', <xsl:value-of select="$patientid"/>, <xsl:value-of select="$admitid"/>);
      </xsl:attribute>
      <xsl:attribute name="onmouseover">
        javascript:this.style.cursor='pointer';
      </xsl:attribute>
      <xsl:attribute name="onmouseout">
        javascript:this.style.cursor='';
      </xsl:attribute>
    </xsl:when>
    <xsl:when test ="$value = 'Ok'">
      <xsl:attribute name="style">
        color:blue;width:63px
      </xsl:attribute>
      <xsl:attribute name="onclick">
        javascript:BSForm_onclick('<xsl:value-of select="$form"/>', '<xsl:value-of select="$year"/>', '<xsl:value-of select="$value"/>', <xsl:value-of select="$patientid"/>);
      </xsl:attribute>
      <xsl:attribute name="onmouseover">
        javascript:this.style.cursor='pointer';
      </xsl:attribute>
      <xsl:attribute name="onmouseout">
        javascript:this.style.cursor='';
      </xsl:attribute>
    </xsl:when>
    <xsl:otherwise>
      <xsl:attribute name="style">
        color:white;width:63px
      </xsl:attribute>
    </xsl:otherwise>
  </xsl:choose>
</xsl:template>
  
</xsl:stylesheet> 

