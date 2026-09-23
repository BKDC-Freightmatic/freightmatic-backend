import { Controller, Get, Post, Body, Request, UseGuards } from '@nestjs/common';
import { ApiTags, ApiOperation, ApiBearerAuth } from '@nestjs/swagger';
import { AuthService } from './auth.service';
import { RegisterDto } from './dto/register.dto';
import { LoginDto } from './dto/login.dto';
import { Public } from '../../common/decorators/public.decorator';

@ApiTags('auth')
@Controller('v1/auth')
export class AuthController {
  constructor(private readonly authService: AuthService) {}

  @Public()
  @Post('sign-up')
  @ApiOperation({ summary: 'Register a new user' })
  async signUp(@Body() dto: RegisterDto) {
    return this.authService.register(dto);
  }

  @Public()
  @Post('sign-in')
  @ApiOperation({ summary: 'Sign in user' })
  async signIn(@Body() dto: LoginDto) {
    return this.authService.login(dto);
  }

  @Public()
  @Get('access-token')
  @ApiOperation({ summary: 'Refresh access token' })
  async getAccessToken(@Request() req) {
    return this.authService.refreshAccessToken(req.user?.userId);
  }

  @ApiBearerAuth()
  @Post('sign-out')
  @ApiOperation({ summary: 'Sign out user' })
  async signOut(@Request() req) {
    return this.authService.logout(req.user.userId);
  }
}
