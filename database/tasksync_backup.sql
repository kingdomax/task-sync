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
-- Name: tasks; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tasks (
    id integer NOT NULL,
    title text NOT NULL,
    assignee_id integer,
    project_id integer NOT NULL,
    status text DEFAULT 'backlog'::text NOT NULL,
    last_modified timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
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
\.


--
-- Data for Name: projects; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.projects (id, name) FROM stdin;
1	Project Artemis
\.


--
-- Data for Name: tasks; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.tasks (id, title, assignee_id, project_id, status, last_modified) FROM stdin;
6	Use SignalR to enable real-time updates across team boards	1	1	done	2025-05-19 20:41:12.509355
2	CRUD operation for project & task management	1	1	done	2025-05-19 20:29:05.783776
5	Comments support on tasks	\N	1	backlog	2025-06-27 14:45:05.398216
8	Award points for task completion and other contributions	\N	1	backlog	2025-06-28 14:59:32.466135
9	Display a leaderboard to encourage friendly competition\n	\N	1	backlog	2025-05-19 20:38:17.672711
1	Implement authentication flow with JWT	2	1	done	2025-05-19 21:07:42.768689
4	File attachment support on task	2	1	todo	2025-05-19 21:09:34.549618
7	Notification system for task updates, assignments, and comments	\N	1	todo	2025-06-08 15:15:42.175616
3	Drag-and-drop functionality for moving tasks between columns	3	1	inprogress	2025-06-26 19:07:14.53935
\.


--
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.users (id, username, firstname, lastname, email, password, password_plain, points) FROM stdin;
3	jaydonk	Jaydon	Frankie	jaydon.frankie@tasksync.com	"$2a$11$vSfdyOcyYnvfVSaocVb/0ehSahhjrYWx0O6fc9FRsPuqTXeo9aH3y"	\N	0
2	kingdomax	Kingdomax	SNPP	kingdomax@tasksync.com	$2a$11$vSfdyOcyYnvfVSaocVb/0ehSahhjrYWx0O6fc9FRsPuqTXeo9aH3y	\N	0
1	admin	Pramoch	Viriyathomrongul	admin@tasksync.com	$2a$11$cSD05UaF0084NVGY6Iv5KuGrJGEmbPV/i98QqojmGlCyIdr0xe6i6	123456789	12
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

SELECT pg_catalog.setval('public.points_log_id_seq', 6, true);


--
-- Name: projects_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.projects_id_seq', 1, false);


--
-- Name: tasks_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.tasks_id_seq', 44, true);


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
-- Name: idx_title; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_title ON public.tasks USING btree (title);


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

