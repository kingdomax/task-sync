--
-- PostgreSQL database dump
--

-- Dumped from database version 17.5 (Debian 17.5-1.pgdg120+1)
-- Dumped by pg_dump version 17.5 (Debian 17.5-1.pgdg120+1)

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: points_log; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.points_log (
    id integer NOT NULL,
    user_id integer NOT NULL,
    task_id integer,
    points_awarded integer NOT NULL,
    created_at timestamp without time zone DEFAULT now() NOT NULL,
    reason character varying NOT NULL
);


ALTER TABLE public.points_log OWNER TO postgres;

--
-- Name: points_log_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.points_log_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.points_log_id_seq OWNER TO postgres;

--
-- Name: points_log_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.points_log_id_seq OWNED BY public.points_log.id;


--
-- Name: projects; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.projects (
    id integer NOT NULL,
    name text NOT NULL
);


ALTER TABLE public.projects OWNER TO postgres;

--
-- Name: projects_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.projects_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.projects_id_seq OWNER TO postgres;

--
-- Name: projects_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.projects_id_seq OWNED BY public.projects.id;


--
-- Name: task_comments; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.task_comments (
    id integer NOT NULL,
    task_id integer NOT NULL,
    commenter_id integer,
    created_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    type text NOT NULL,
    comment_text text NOT NULL,
    metadata jsonb,
    CONSTRAINT task_comments_type_check CHECK ((type = ANY (ARRAY['user_comment'::text, 'status_change'::text, 'assignment_change'::text, 'title_change'::text, 'description_change'::text, 'task_created'::text])))
);


ALTER TABLE public.task_comments OWNER TO postgres;

--
-- Name: task_comments_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.task_comments_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.task_comments_id_seq OWNER TO postgres;

--
-- Name: task_comments_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.task_comments_id_seq OWNED BY public.task_comments.id;


--
-- Name: tasks; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tasks (
    id integer NOT NULL,
    title text NOT NULL,
    assignee_id integer,
    project_id integer NOT NULL,
    status text DEFAULT 'backlog'::text NOT NULL,
    last_modified timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    creator_id integer,
    CONSTRAINT tasks_status_check CHECK ((status = ANY (ARRAY['backlog'::text, 'todo'::text, 'inprogress'::text, 'done'::text])))
);


ALTER TABLE public.tasks OWNER TO postgres;

--
-- Name: tasks_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.tasks_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.tasks_id_seq OWNER TO postgres;

--
-- Name: tasks_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.tasks_id_seq OWNED BY public.tasks.id;


--
-- Name: user_points_summary; Type: VIEW; Schema: public; Owner: postgres
--

CREATE VIEW public.user_points_summary AS
 SELECT user_id,
    count(*) AS records,
    sum(points_awarded) AS total_points
   FROM public.points_log
  GROUP BY user_id;


ALTER VIEW public.user_points_summary OWNER TO postgres;

--
-- Name: users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.users (
    id integer NOT NULL,
    username text NOT NULL,
    firstname text NOT NULL,
    lastname text NOT NULL,
    email text NOT NULL,
    password text NOT NULL,
    password_plain text,
    points integer DEFAULT 0
);


ALTER TABLE public.users OWNER TO postgres;

--
-- Name: users_projects; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.users_projects (
    user_id integer NOT NULL,
    project_id integer NOT NULL
);


ALTER TABLE public.users_projects OWNER TO postgres;

--
-- Name: points_log id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.points_log ALTER COLUMN id SET DEFAULT nextval('public.points_log_id_seq'::regclass);


--
-- Name: projects id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.projects ALTER COLUMN id SET DEFAULT nextval('public.projects_id_seq'::regclass);


--
-- Name: task_comments id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.task_comments ALTER COLUMN id SET DEFAULT nextval('public.task_comments_id_seq'::regclass);


--
-- Name: tasks id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tasks ALTER COLUMN id SET DEFAULT nextval('public.tasks_id_seq'::regclass);


--
-- Data for Name: points_log; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.points_log (id, user_id, task_id, points_awarded, created_at, reason) FROM stdin;
1	1	8	3	2025-06-27 14:16:39.81779	completed_task
2	1	\N	1	2025-06-27 14:17:53.158261	created_task
3	1	\N	3	2025-06-27 14:41:24.235284	completed_task
4	1	5	3	2025-06-27 14:45:03.124921	completed_task
5	1	8	3	2025-06-27 14:49:00.045024	completed_task
6	1	8	3	2025-06-28 14:59:26.064269	completed_task
7	1	\N	1	2025-06-29 16:07:18.048193	created_task
10	1	\N	1	2025-06-29 16:14:03.931949	created_task
8	1	\N	1	2025-06-29 16:13:52.472555	created_task
9	1	\N	1	2025-06-29 16:13:57.770498	created_task
11	1	3	3	2025-06-29 16:14:40.775122	completed_task
12	1	6	3	2025-06-29 16:20:30.617692	completed_task
13	1	3	3	2025-06-30 12:44:44.236825	completed_task
14	1	49	1	2025-06-30 14:25:03.588809	created_task
15	1	49	3	2025-07-02 13:20:01.356227	completed_task
16	1	\N	1	2025-07-07 10:57:07.093934	created_task
17	1	\N	1	2025-07-07 11:02:20.911722	created_task
\.


--
-- Data for Name: projects; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.projects (id, name) FROM stdin;
1	Artemis
\.


--
-- Data for Name: task_comments; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.task_comments (id, task_id, commenter_id, created_at, type, comment_text, metadata) FROM stdin;
1	6	1	2025-05-19 13:20:01.333996	task_created	admin added this task to project Artemis	{"event": "task_created", "task_id": 6, "username": "admin", "project_id": 1, "assignee_id": 1, "project_name": "Artemis"}
2	3	3	2025-05-19 13:20:01.333996	task_created	jaydonk added this task to project Artemis	{"event": "task_created", "task_id": 3, "username": "jaydonk", "project_id": 1, "assignee_id": 3, "project_name": "Artemis"}
3	5	1	2025-05-19 13:20:01.333996	task_created	admin added this task to project Artemis	{"event": "task_created", "task_id": 5, "username": "admin", "project_id": 1, "assignee_id": 1, "project_name": "Artemis"}
4	2	1	2025-05-19 13:20:01.333996	task_created	admin added this task to project Artemis	{"event": "task_created", "task_id": 2, "username": "admin", "project_id": 1, "assignee_id": 1, "project_name": "Artemis"}
5	8	1	2025-05-19 13:20:01.333996	task_created	admin added this task to project Artemis	{"event": "task_created", "task_id": 8, "username": "admin", "project_id": 1, "assignee_id": 1, "project_name": "Artemis"}
6	7	1	2025-05-19 13:20:01.333996	task_created	admin added this task to project Artemis	{"event": "task_created", "task_id": 7, "username": "admin", "project_id": 1, "assignee_id": 1, "project_name": "Artemis"}
7	49	1	2025-05-19 13:20:01.333996	task_created	admin added this task to project Artemis	{"event": "task_created", "task_id": 49, "username": "admin", "project_id": 1, "assignee_id": 1, "project_name": "Artemis"}
8	9	1	2025-05-19 13:20:01.333996	task_created	admin added this task to project Artemis	{"event": "task_created", "task_id": 9, "username": "admin", "project_id": 1, "assignee_id": 1, "project_name": "Artemis"}
9	1	2	2025-05-19 13:20:01.333996	task_created	kingdomax added this task to project Artemis	{"event": "task_created", "task_id": 1, "username": "kingdomax", "project_id": 1, "assignee_id": 2, "project_name": "Artemis"}
10	4	2	2025-05-19 13:20:01.333996	task_created	kingdomax added this task to project Artemis	{"event": "task_created", "task_id": 4, "username": "kingdomax", "project_id": 1, "assignee_id": 2, "project_name": "Artemis"}
\.


--
-- Data for Name: tasks; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.tasks (id, title, assignee_id, project_id, status, last_modified, creator_id) FROM stdin;
6	Use SignalR to enable real-time updates across team boards	1	1	done	2025-06-29 16:20:30.606068	1
3	Drag-and-drop functionality for moving tasks between columns	3	1	done	2025-06-30 12:44:44.10735	1
5	Comments support on tasks	\N	1	todo	2025-06-30 12:45:08.998863	1
2	CRUD operation for project & task management	1	1	done	2025-05-19 20:29:05.783776	1
8	Award points for task completion and other contributions	\N	1	backlog	2025-07-02 13:18:11.748087	1
7	Notification system for task updates, assignments, and comments	\N	1	backlog	2025-07-02 13:19:31.211842	1
49	Display task item detail	1	1	done	2025-07-02 13:20:01.333996	1
9	Display a leaderboard to encourage friendly competition\n	\N	1	inprogress	2025-07-02 13:20:16.980736	1
1	Implement authentication flow with JWT	2	1	done	2025-05-19 21:07:42.768689	1
4	File attachment support on task	2	1	todo	2025-05-19 21:09:34.549618	1
\.


--
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.users (id, username, firstname, lastname, email, password, password_plain, points) FROM stdin;
3	jaydonk	Jaydon	Frankie	jaydon.frankie@tasksync.com	"$2a$11$vSfdyOcyYnvfVSaocVb/0ehSahhjrYWx0O6fc9FRsPuqTXeo9aH3y"	\N	0
2	kingdomax	Kingdomax	SNPP	kingdomax@tasksync.com	$2a$11$vSfdyOcyYnvfVSaocVb/0ehSahhjrYWx0O6fc9FRsPuqTXeo9aH3y	\N	0
1	admin	Pramoch	Viriyathomrongul	admin@tasksync.com	$2a$11$cSD05UaF0084NVGY6Iv5KuGrJGEmbPV/i98QqojmGlCyIdr0xe6i6	123456789	31
\.


--
-- Data for Name: users_projects; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.users_projects (user_id, project_id) FROM stdin;
1	1
2	1
\.


--
-- Name: points_log_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.points_log_id_seq', 17, true);


--
-- Name: projects_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.projects_id_seq', 1, false);


--
-- Name: task_comments_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.task_comments_id_seq', 11, true);


--
-- Name: tasks_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.tasks_id_seq', 51, true);


--
-- Name: points_log points_log_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.points_log
    ADD CONSTRAINT points_log_pkey PRIMARY KEY (id);


--
-- Name: projects projects_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.projects
    ADD CONSTRAINT projects_pkey PRIMARY KEY (id);


--
-- Name: task_comments task_comments_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.task_comments
    ADD CONSTRAINT task_comments_pkey PRIMARY KEY (id);


--
-- Name: tasks tasks_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tasks
    ADD CONSTRAINT tasks_pkey PRIMARY KEY (id);


--
-- Name: users users_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (id);


--
-- Name: users_projects users_projects_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users_projects
    ADD CONSTRAINT users_projects_pkey PRIMARY KEY (user_id, project_id);


--
-- Name: idx_task_comments_created_at; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_task_comments_created_at ON public.task_comments USING btree (created_at DESC);


--
-- Name: idx_task_comments_task_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_task_comments_task_id ON public.task_comments USING btree (task_id);


--
-- Name: idx_title; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_title ON public.tasks USING btree (title);


--
-- Name: tasks fk_creator; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tasks
    ADD CONSTRAINT fk_creator FOREIGN KEY (creator_id) REFERENCES public.users(id);


--
-- Name: points_log fk_pointslog_task; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.points_log
    ADD CONSTRAINT fk_pointslog_task FOREIGN KEY (task_id) REFERENCES public.tasks(id) ON DELETE SET NULL;


--
-- Name: points_log fk_pointslog_user; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.points_log
    ADD CONSTRAINT fk_pointslog_user FOREIGN KEY (user_id) REFERENCES public.users(id) ON DELETE CASCADE;


--
-- Name: task_comments task_comments_commenter_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.task_comments
    ADD CONSTRAINT task_comments_commenter_id_fkey FOREIGN KEY (commenter_id) REFERENCES public.users(id) ON DELETE SET NULL;


--
-- Name: task_comments task_comments_task_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.task_comments
    ADD CONSTRAINT task_comments_task_id_fkey FOREIGN KEY (task_id) REFERENCES public.tasks(id) ON DELETE CASCADE;


--
-- Name: tasks tasks_assignee_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tasks
    ADD CONSTRAINT tasks_assignee_id_fkey FOREIGN KEY (assignee_id) REFERENCES public.users(id) ON DELETE SET NULL;


--
-- Name: tasks tasks_project_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tasks
    ADD CONSTRAINT tasks_project_id_fkey FOREIGN KEY (project_id) REFERENCES public.projects(id) ON DELETE CASCADE;


--
-- Name: users_projects users_projects_project_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users_projects
    ADD CONSTRAINT users_projects_project_id_fkey FOREIGN KEY (project_id) REFERENCES public.projects(id) ON DELETE CASCADE;


--
-- Name: users_projects users_projects_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users_projects
    ADD CONSTRAINT users_projects_user_id_fkey FOREIGN KEY (user_id) REFERENCES public.users(id) ON DELETE CASCADE;


--
-- PostgreSQL database dump complete
--

