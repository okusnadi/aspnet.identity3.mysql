create table `roleclaim`(
	`id` int auto_increment not null,
	`claim_type` varchar(1024) null,
	`claim_value` varchar(1024) null,	
	`role_id` varchar(128) null,
	constraint `pk_role_claim` primary key (`id` asc)
)engine=InnoDB, default character set = `utf8`;

create table `role`(
	`id` varchar(128) not null,	
	`name` varchar(256) null,
    `normalized_name` varchar(256) null,
    `concurrency_stamp` varchar(1024) null,	
	constraint `pk_role` primary key (`id` asc)
)engine=InnoDB, default character set = `utf8`;

create table `userclaim`(
	`id` int auto_increment not null,
	`claim_type` varchar(1024) null,
	`claim_value` varchar(1024) null,	
	`user_id` varchar(128) null,
	constraint `pk_user_claim` primary key (`id` asc)
)engine=InnoDB, default character set = `utf8`;

create table `userlogin`(
	`login_provider` varchar(128) not null,	
	`provider_key` varchar(128) not null,
    `provider_displayname` varchar(512) null,
	`user_id` varchar(128) null,
	constraint `pk_user_login` primary key (`login_provider` asc,`provider_key` asc)
)engine=InnoDB, default character set = `utf8`;

create table `userrole`(
	`role_id` varchar(128) not null,
	`user_id` varchar(128) not null,
	constraint `pk_user_role` primary key (`user_id` asc,`role_id` asc)
)engine=InnoDB, default character set = `utf8`;

create table `user`(
	`id` varchar(128) not null,
    `username` varchar(256) null,	
	`email` varchar(256) null,
	`email_confirmed` bit not null,	
	`normalized_email` varchar(256) null,
	`normalized_username` varchar(256) null,
	`password_hash` varchar(1024) null,
	`phone_number` varchar(128) null,
	`phone_number_confirmed` bit not null,
    `access_failed_count` int not null,
	`concurrency_stamp` varchar(1024) null,
	`lockout_enabled` bit not null,
	`lockout_end` timestamp null,
	`security_stamp` varchar(1024) null,
	`two_factor_enabled` bit not null,	
	constraint `pk_user` primary key (`id` asc)
)engine=InnoDB, default character set = `utf8`;

alter table `roleclaim` add constraint `fk_roleclaim_role_id` foreign key(`role_id`)
references `role` (`id`);

alter table `userclaim`  add constraint `fk_userclaim_user_id` foreign key(`user_id`)
references `user` (`id`);

alter table `userlogin`  add constraint `fk_userlogin_user_id` foreign key(`user_id`)
references `user` (`id`);

alter table `userrole`  add constraint `fk_userrole_role_id` foreign key(`role_id`)
references `role` (`id`);

alter table `userrole`  add constraint `fk_userrole_user_id` foreign key(`user_id`)
references `user` (`id`);
