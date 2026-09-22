import { Controller, Get, Post, Put, Param, Body, Request } from '@nestjs/common';
import { ApiTags, ApiOperation, ApiBearerAuth } from '@nestjs/swagger';
import { UsersService } from './users.service';

@ApiTags('users')
@ApiBearerAuth()
@Controller('v1/users')
export class UsersController {
  constructor(private readonly usersService: UsersService) {}

  @Get('me')
  @ApiOperation({ summary: 'Get current logged in user profile' })
  async getMe(@Request() req) {
    return this.usersService.findOne(req.user.userId);
  }

  @Get('partial-trucks')
  @ApiOperation({ summary: 'Get partial truck users' })
  async getPartialTrucks() {
    return this.usersService.getPartialTrucks();
  }

  @Get('full-trucks')
  @ApiOperation({ summary: 'Get full truck users' })
  async getFullTrucks() {
    return this.usersService.getFullTrucks();
  }

  @Get('top-truckers')
  @ApiOperation({ summary: 'Get top rated truckers' })
  async getTopTruckers() {
    return this.usersService.getTopTruckers();
  }

  @Post('suitable-truckers')
  @ApiOperation({ summary: 'Get AI suitable truckers for route' })
  async getSuitableTruckersAI(@Body() dto: any) {
    return this.usersService.getSuitableTruckersAI(dto);
  }

  @Get(':id')
  @ApiOperation({ summary: 'Get user by ID' })
  async findOne(@Param('id') id: string) {
    return this.usersService.findOne(id);
  }

  @Get()
  @ApiOperation({ summary: 'Get all users' })
  async findAll() {
    return this.usersService.findAll();
  }

  @Put('me')
  @ApiOperation({ summary: 'Update current user profile' })
  async updateMe(@Request() req, @Body() updateData: any) {
    return this.usersService.update(req.user.userId, updateData);
  }
}
