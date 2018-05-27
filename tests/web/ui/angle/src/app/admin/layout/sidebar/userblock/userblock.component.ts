import { Component, OnInit } from '@angular/core';

import { UserblockService } from './userblock.service';
import { AuthTokenService } from '../../../../shared/osharp/services/auth-token.service';
import { osharp } from '../../../../shared/osharp';

@Component({
  selector: 'admin-userblock',
  templateUrl: './userblock.component.html',
  styleUrls: ['./userblock.component.scss']
})
export class UserblockComponent implements OnInit {
  user: any;
  constructor(public userblockService: UserblockService) {

    this.user = {
      picture: 'assets/img/user/02.jpg',
      name: '游客',
      remark: '游客'
    };
  }

  ngOnInit() {
    let authUser = osharp.Auth.user();
    if (authUser == null) {
      return null;
    }
    this.user.name = authUser.NickName || authUser.UserName;
    this.user.remark = osharp.Tools.expandAndToString(authUser.Roles, ",");
  }

  userBlockIsVisible() {
    return this.userblockService.getVisibility();
  }

}
